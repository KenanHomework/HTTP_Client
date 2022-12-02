using Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Classes
{

    [ReuqestlCLass]
    public class Car
    {

        [Command(CallName = "GET", Info = "For get All Cars")]
        public const string GetCommand = "GET";

        [Command(CallName = "PUT", Info = "For add Car", Parametres = "Car instance", Tricks = "Without ID")]
        public const string PutCommand = "PUT";

        [Command(CallName = "POST", Info = "For update Car", Parametres = "int idOfCarToBeChanged ,Car instance", Tricks = "With ID")]
        public const string PostCommand = "POST";

        [Command(CallName = "DEL", Info = "For delete Car", Parametres = "Int idOfCarToBeDeleted")]
        public const string DeleteCommand = "DEL";


        [Property]
        [RequiredID]
        public int Id { get; set; }

        [Property]
        [Required]
        public string Name { get; set; }

        [Property]
        [Required]
        public string Vendor { get; set; }

        public bool Same(Car other) => (other.Id == this.Id) && (other.Name == this.Name) && (other.Vendor == this.Vendor);

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"\nID: {Id}");
            builder.Append($"\nName: {Name}");
            builder.Append($"\nVendor: {Vendor}");

            return builder.ToString();
        }

        public override bool Equals(object? obj)
        {
            if ( obj is Car other)
            {
                return (other.Name == this.Name) && (other.Vendor == this.Vendor);
            }

            return base.Equals(obj);
        }

    }
}
