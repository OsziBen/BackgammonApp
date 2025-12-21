using Common.Exceptions;

namespace Application.Shared
{
    public static class RepositoryExtensions
    {
        public static async Task<T> GetOrThrowAsync<T>(
            this Task<T?> task,
            string resourceName,
            object key)
            where T : class
        {
            var entity = await task;

            if (entity == null)
            {
                throw NotFoundException.CreateForResource(
                    resourceName,
                    key);
            }

            return entity;
        }
    }
}
