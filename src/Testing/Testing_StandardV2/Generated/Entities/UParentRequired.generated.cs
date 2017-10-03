//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Testing
{
   public partial class UParentRequired
   {
      partial void Init();

      /// <summary>
      /// Default constructor. Protected due to required properties, but present because EF needs it.
      /// </summary>
      protected UParentRequired()
      {
         UChildCollection = new HashSet<UChild>();

         Init();
      }

      /// <summary>
      /// Public constructor with required data
      /// </summary>
      /// <param name="_uchildrequired"></param>
      public UParentRequired(UChild _uchildrequired)
      {
         if (_uchildrequired == null) throw new ArgumentNullException(nameof(_uchildrequired));
         UChildRequired = _uchildrequired;

         UChildCollection = new HashSet<UChild>();
      }

      /// <summary>
      /// Static create function (for use in LINQ queries, etc.)
      /// </summary>
      /// <param name="_uchildrequired"></param>
      public static UParentRequired Create(UChild _uchildrequired)
      {
         return new UParentRequired(_uchildrequired);
      }

      // Persistent properties

      /// <summary>
      /// Identity, Required, Indexed
      /// </summary>
      public int Id { get; set; }

      // Persistent navigation properties

      public UChild UChildRequired { get; set; }  // Required
      public ICollection<UChild> UChildCollection { get; set; } 
      public UChild UChildOptional { get; set; } 
   }
}

