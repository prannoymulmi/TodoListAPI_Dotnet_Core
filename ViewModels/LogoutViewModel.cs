using System.ComponentModel.DataAnnotations;

namespace ListsWebAPi.ViewModels
{
    public class LogoutViewModel
    {
        [Required] 
        [DataType(DataType.Text)]
        public string token;
    }
}