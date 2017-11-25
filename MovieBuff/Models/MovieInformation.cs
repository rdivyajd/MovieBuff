using Microsoft.Bot.Builder.FormFlow;
using System;

namespace MovieBuff.Models
{
    [Serializable]
    public class MovieInformation
    {
        public static IForm<MovieInformation> BuildForm()
        {
            return new FormBuilder<MovieInformation>().Build();
        }
    }
}