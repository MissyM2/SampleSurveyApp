using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SampleSurveyApp.Core.Validation
{
    public partial class ValidatableObject<T> : ObservableObject
    {
        public List<IValidationRule<T>> Validations { get; }

        [ObservableProperty]
        List<string> _errors;

        [ObservableProperty]
        T _value;

        [ObservableProperty]
        bool _isValid;

        public ValidatableObject()
        {
            _isValid = true;
            _errors = new List<string>();
            Validations = new List<IValidationRule<T>>();
        }

        public bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = Validations.Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            return IsValid;
        }
    }
}
