namespace BlogManagementProject.ViewModels
{
    public class UpdateProfileViewModel
    {
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }

        public IFormFile? ProfileImage { get; set; }
        public string? ExistingImagePath { get; set; }
    }
}
