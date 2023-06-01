using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HCI.Tools
{
    public class RatingToStarsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double rating)
            {
                int roundedRating = (int)Math.Round(rating);
                var ratingStars = new List<object>();

                for (int i = 1; i <= 5; i++)
                {
                    if (i <= roundedRating)
                        ratingStars.Add(new object());
                    else
                        ratingStars.Add(null);
                }

                return ratingStars;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
