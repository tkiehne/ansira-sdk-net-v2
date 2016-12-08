using System;
using System.Collections.Generic;
using System.Configuration;
using Ansira;
using Ansira.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;

namespace AnsiraSDKTests
{
    /// <summary>
    ///This is a test class for Ansira Objects
    ///</summary>
    [TestClass]
    public class AnsiraObjectTests
    {
        public bool TryValidate(object @object, out List<ValidationResult> results)
        {
            var context = new ValidationContext(@object, serviceProvider: null, items: null);
            results = new List<ValidationResult>();
            return Validator.TryValidateObject(
                @object, context, results,
                validateAllProperties: true
            );
        }

        public void WriteValidationResults(List<ValidationResult> results)
        {
            if (results.Count > 0)
            {
                foreach (var validationResult in results)
                {
                    Console.WriteLine(validationResult.ErrorMessage);
                }
            }
        }

        [TestMethod()]
        public void ObjectBreed()
        {
            var results = new List<ValidationResult>();

            Breed breed = new Breed() { Id=1, KeyName="TEST", Name="Validate", PetType=null };
            Assert.IsTrue(TryValidate(breed, out results));
            WriteValidationResults(results);

            var old = breed.KeyName;
            breed.KeyName = "bad key value";
            Assert.IsFalse(TryValidate(breed, out results));
            WriteValidationResults(results);
            breed.KeyName = old;

            old = breed.Name;
            breed.Name = "Apparently, names can't be like this";
            Assert.IsFalse(TryValidate(breed, out results));
            WriteValidationResults(results);
            breed.Name = old;
        }

        [TestMethod()]
        public void ObjectPet()
        {
            var results = new List<ValidationResult>();

            Pet pet = new Pet() { Id = 1, Name="test", Gender="male", Size="large", Color="brown", ImageUrl= "https://www.purina.com/media/284062/Akitas_2913.jpg/560/0/center/middle", AcquisitionMethod="test", DiscoveryMethod="test", DiscoveryMethodDetail="test" };
            Assert.IsTrue(TryValidate(pet, out results));
            WriteValidationResults(results);

            var old = pet.Name;
            pet.Name = "names- can't \"be\" like, this.";
            Assert.IsFalse(TryValidate(pet, out results));
            WriteValidationResults(results);
            pet.Name = old;

            old = pet.Gender;
            pet.Gender = "femalegender";
            Assert.IsFalse(TryValidate(pet, out results));
            WriteValidationResults(results);
            pet.Gender = old;

            old = pet.Size;
            pet.Size = "not. \"a\" 'valid' size.";
            Assert.IsFalse(TryValidate(pet, out results));
            WriteValidationResults(results);
            pet.Size = old;

            old = pet.Color;
            pet.Color = "not, \"a\" 'color' .";
            Assert.IsFalse(TryValidate(pet, out results));
            WriteValidationResults(results);
            pet.Color = old;

            old = pet.ImageUrl;
            pet.ImageUrl = "notAUrl";
            Assert.IsFalse(TryValidate(pet, out results));
            WriteValidationResults(results);
            pet.ImageUrl = old;
        }
    }
}
