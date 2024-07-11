using _3_MVCCRUDUSINGCRUD.Common;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace _3_MVCCRUDUSINGCRUD.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
        [DisplayName("Conform Email")]
        [System.ComponentModel.DataAnnotations.Compare("Email",ErrorMessage ="Email and confirm email is same")]
        public string conformEmail { get; set; }
    }

    public class UserMetaData
    {
        [Required(ErrorMessage ="Please Enter Name")]
        [StringLength(20,MinimumLength =3,ErrorMessage ="Name should be at least 3 or 20 character long")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please Enter Age")]
        [Range(1,150)]
        public Nullable<int> Age { get; set; }
        [Required(ErrorMessage = "Please Enter Mobile")]
        [RegularExpression("\\d{10}", ErrorMessage = "please Enter 10 dogit mobile number")]
        public string Mobile { get; set; }
      
        [Required(ErrorMessage = "Please Enter Email")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Please enter valid email")]
        [Remote("IsEmailExists","Account",ErrorMessage="Email Already used")]
        public string Email { get; set; }

        [DisplayName("Birthday Date")]
        [Required(ErrorMessage = "Please Enter DataOFBirth")]
       // [DataType(DataType.Date,ErrorMessage ="Please Enter valid date")]
        [DateRange(ErrorMessage ="Date of birth should be less than todays date")]
        [DisplayFormat(DataFormatString ="0:dd/mm/yyyy")]
        public Nullable<System.DateTime> DateOfBirth { get; set; }

    }
}
