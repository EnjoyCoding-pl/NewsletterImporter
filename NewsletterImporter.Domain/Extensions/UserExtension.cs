using System;
using System.Collections.Generic;
using NewsletterImporter.Domain.Models;

namespace NewsletterImporter.Domain.Extensions
{
    public static class UserExtension
    {
        public static bool ShouldAdd(this (User Left, User Right) item) => item.Left is not null && item.Right is null;
        public static bool ShouldDelete(this (User Left, User Right) item) => item.Left is null && item.Right is not null;
           
        public static async IAsyncEnumerable<(User Left, User Right)> ZipUsersByEmail(this IAsyncEnumerable<User> leftSource, IAsyncEnumerable<User> rightSource)
        {
            var leftEnumerator = leftSource.GetAsyncEnumerator();
            var rightEnumerator = rightSource.GetAsyncEnumerator();

            bool leftHasNext = await leftEnumerator.MoveNextAsync();
            bool rightHasNext = await rightEnumerator.MoveNextAsync();

            while (leftHasNext || rightHasNext)
            {
                var leftItem = leftEnumerator.Current;
                var rightItem = rightEnumerator.Current;

                var compareResult = StringComparer.CurrentCultureIgnoreCase.Compare(leftItem?.Email, rightItem?.Email);

                if (compareResult == 0)
                {
                    if (leftHasNext)
                        leftHasNext = await leftEnumerator.MoveNextAsync();

                    if (rightHasNext)
                        rightHasNext = await rightEnumerator.MoveNextAsync();

                    yield return (leftItem, rightItem);
                }
                else if ((compareResult < 0 && leftHasNext) || (leftHasNext && !rightHasNext))
                {
                    leftHasNext = await leftEnumerator.MoveNextAsync();
                    yield return (leftItem, null);
                }
                else if ((compareResult > 0 && rightHasNext) || (rightHasNext && !leftHasNext))
                {
                    rightHasNext = await rightEnumerator.MoveNextAsync();
                    yield return (null, rightItem);
                }
                else
                {
                    yield break;
                }
            }
        }
 
    }
}