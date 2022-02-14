using SQLite;
using System;

namespace Wordle.Models
{
    public class Settings
    {
        public enum FormFieldType
        {
            Boolean, Integer, String
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; }

        [Indexed(Name = "UniqueFormField", Order = 1, Unique = true)]
        public int FormId { get; set; }

        [Indexed(Name = "UniqueFormField", Order = 2, Unique = true)]
        public string Name { get; set; }

        public FormFieldType FieldType { get; set; }

        public int IntValue { get; set; }

        public Boolean BooleanValue { get; set; }

        public string StringValue { get; set; }

        public static Settings CreateSettingsModel(int formId, string name, FormFieldType fieldType)
        {
            return new Settings()
            {
                FormId = formId,
                Name = name,
                FieldType = fieldType
            };
        }
    }
}
