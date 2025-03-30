using Core.Entities.Identity;

namespace Core.Specifications.Users
{
    public class UserSpecification : BaseSpecification<ApplicationUser>
    {
        public UserSpecification() : base(x => true)
        {
            AddOrderBy(x => x.Id);
        }

        public UserSpecification(UserParams userParams, bool applyPaging = true)
            : base(u => string.IsNullOrEmpty(userParams.Search) ||
                       u.UserName.ToLower().Contains(userParams.Search.ToLower()) ||
                       u.Email.ToLower().Contains(userParams.Search.ToLower()) ||
                       u.FirstName.ToLower().Contains(userParams.Search.ToLower()) ||
                       u.LastName.ToLower().Contains(userParams.Search.ToLower()) ||
                       u.PhoneNumber.ToLower().Contains(userParams.Search.ToLower()))
        {
            AddOrderByDescending(u => u.CreatedAt);

            if (applyPaging)
            {
                ApplyPaging(userParams.PageSize * (userParams.Page - 1), userParams.PageSize);
            }
        }
    }
}