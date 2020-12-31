using System;
using System.Collections.Generic;

namespace HSTDataLayer
{
    public partial class Person : IEquatable<Person>, IComparable<Person>
    {
        public string FullName => $"{ this.FirstName } { this.LastName }";
        /// <summary>
        /// Override ToString() to control a basic string-output of a Person instance
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{ this.FirstName } { this.LastName } { this.Phone } { this.Email }";
        }

        public string GetFirstAndLastName()
        {
            return $"{ this.FirstName } { this.LastName }";
        }

        /// <summary>
        /// Required IComparable method implementation. Reference for multi-field sorting: https://stackoverflow.com/questions/4501501/custom-sorting-icomparer-on-three-fields
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        int IComparable<Person>.CompareTo(Person other)
        {
            if (other == null)
            {
                return 1;
            }

            int result = this.LastName.CompareTo(other.LastName);
            if (result == 0)
            {
                result = this.FirstName.CompareTo(other.FirstName);
            }

            return result;
        }

        /// <summary>
        /// Required IEquatable method implementation.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        bool IEquatable<Person>.Equals(Person other)
        {
            if (other == null)
            {
                return false;
            }

            if (this.FirstName == other.FirstName &&
                this.LastName == other.LastName)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Required Equals() override when implementing IEquitable T (IDE generated code)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj is Person person &&
                   FirstName == person.FirstName &&
                   LastName == person.LastName;
        }

        /// <summary>
        /// Recommended GetHashCode() override when overriding Equals() (IDE generated code]
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int hashCode = -1969988406;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FirstName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(LastName);
            //hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Phone);
            //hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Email);
            return hashCode;
        }

        /// <summary>
        /// Recommended Equal Operator override. Supports IEquatable implementation and Equals() override methods.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Person left, Person right)
        {
            return EqualityComparer<Person>.Default.Equals(left, right);
        }

        /// <summary>
        /// Recommended Not Equal Operator override. Supports IEquatable implementation and Equals() override methods.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Person left, Person right)
        {
            return !(left == right);
        }
    }
}
