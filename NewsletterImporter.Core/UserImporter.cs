using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using NewsletterImporter.Domain.Extensions;
using Open.ChannelExtensions;
using NewsletterImporter.Core.Abstract;
using NewsletterImporter.Core.Interfaces;
using NewsletterImporter.Domain.Models;

namespace NewsletterImporter.Core
{
    public class UserImporter
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailValidationGateway _emailValidatorGateway;
        private readonly INewsletterStorage _newsletterStorage;
        public UserImporter(IUserRepository userRepository, IEmailValidationGateway emailValidatorGateway, INewsletterStorage newsletterStorage)
        {
            _userRepository = userRepository;
            _emailValidatorGateway = emailValidatorGateway;
            _newsletterStorage = newsletterStorage;
        }

        public async Task ImportUsers()
        {
            var deleteChannel = Channel.CreateUnbounded<User>();
            var addChannel = Channel.CreateUnbounded<User>();

            var source = _userRepository.GetSignedUsersAsync()
            .ZipUsersByEmail(_newsletterStorage.GetUsersAsync())
            .ToChannel()
            .ReadAllAsync(async item => await WriteToActionChannel(item, addChannel, deleteChannel));

            var deleteReader = deleteChannel
            .Reader
            .Batch(1000)
            .ReadAllAsync(async x => await _newsletterStorage.DeleteUsersAsync(x));

            var addReader = addChannel.Reader
            .PipeAsync(async user => (User: user, Status: await _emailValidatorGateway.CheckAsync(user.Email)))
            .Filter(x => x.Status.IsValid)
            .Batch(1000)
            .ReadAllAsync(async users => await _newsletterStorage.AddUsersAsync(users));

            await source;

            deleteChannel.Writer.Complete();
            addChannel.Writer.Complete();

            await deleteReader;
            await addReader;
        }
        private async Task WriteToActionChannel((User Left, User Right) item, ChannelWriter<User> addChannel, ChannelWriter<User> deleteChannel)
        {
            if (item.ShouldAdd())
                await addChannel.WriteAsync(item.Left);

            else if (item.ShouldDelete())
                await deleteChannel.WriteAsync(item.Right);
        }
    }
}