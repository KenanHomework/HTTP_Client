using Common.Attributes;
using Common.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Controllers
{
    public static class Controller
    {

        public static bool Check(Car? car)
        {
            if (car == null) return false;

            Type type = car.GetType();

            foreach (var field in type.GetFields())
            {
                object[] requires = field.GetCustomAttributes(typeof(RequiredAttribute), false);
                object[] requiredId = field.GetCustomAttributes(typeof(RequiredIDAttribute), false);

                if (requiredId.Length > 0)
                {
                    object value = field.GetValue(car);

                    if (value == null) return false;
                }

                if (requiredId.Length > 0)
                {
                    int value = (int)field.GetValue(car);

                    if (value < 0) return false;
                }

            }

            return true;
        }

    }
}
