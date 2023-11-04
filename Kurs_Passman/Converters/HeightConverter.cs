using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Kurs_Passman.Converters
{
    /*
    Конвертеры значений (value converter) также позволяют преобразовать значение из источника привязки к типу, который понятен приемнику привязки. 
   Так как не всегда два связываемых привязкой свойства могут иметь совместимые типы. И в этом случае как раз и нужен конвертер значений.

   Конвертер значений должен реализовать интерфейс System.Windows.Data.IValueConverter. Этот интерфейс определяет два метода: 
   - Convert(), который преобразует пришедшее от привязки значение в тот тип, который понимается приемником привязки
   - ConvertBack(), который выполняет противоположную операцию.

   Оба метода принимают четыре параметра:
   - object value: значение, которое надо преобразовать
   - Type targetType: тип, к которому надо преобразовать значение value
   - object parameter: вспомогательный параметр
   - CultureInfo culture: текущая культура приложения
    */
    public class HeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string p)
            {
                if (value is double v)
                {
                    var result = double.TryParse(p, out double param);
                    if (result)
                    {
                        return v / param;
                    }
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
