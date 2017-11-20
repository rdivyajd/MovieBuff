using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieBuff.Models
{
    public enum MovieGenre
    {
        Action = 1,
        Biography,
        Crime,
        Drama,
        Fantasy,
        Romance,
        Sci_Fi,
        Thriller,
        War,
        Western
    }

    public enum MovieLanguage
    {
        English
    }

    [Serializable]
    public class UserInformation
    {
        public List<MovieGenre> movieGenre;
        public int? userAge;
        public string userName;
        public DateTime? dateOfBirth;
        public string mobileNumber;
        public string movieLanguage;
  
        public static IForm<UserInformation> BuildForm()
        {
            return new FormBuilder<UserInformation>().Message("Welcome to the User Information Form! Please fill out a few details before I make Movie suggestions kindly!")
                .Build();
        }
    }
}