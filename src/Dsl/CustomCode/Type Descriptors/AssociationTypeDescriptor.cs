﻿using System;
using System.ComponentModel;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class AssociationTypeDescriptor
   {
      private DomainDataDirectory storeDomainDataDirectory;

      /// <summary>
      ///    Returns the property descriptors for the described ModelClass domain class, adding tracking property
      ///    descriptor(s).
      /// </summary>
      private PropertyDescriptorCollection GetCustomProperties(Attribute[] attributes)
      {
         // Get the default property descriptors from the base class  
         PropertyDescriptorCollection propertyDescriptors = base.GetProperties(attributes);

         //Add the descriptor for the tracking property.  
         if (ModelElement is Association association)
         {
            storeDomainDataDirectory = association.Store.DomainDataDirectory;

            EFCoreValidator.AdjustEFCoreProperties(propertyDescriptors, association);

            // ImplementNotify implicitly defines autoproperty as false, so we don't display it
            // Similarly, collections are autoproperty == true, so no need to display it then either
            if ((association.Target.ImplementNotify || association.SourceMultiplicity == Multiplicity.ZeroMany) && association is BidirectionalAssociation)
            {
               PropertyDescriptor sourceAutoPropertyDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().SingleOrDefault(x => x.Name == "SourceAutoProperty");
               if (sourceAutoPropertyDescriptor != null) propertyDescriptors.Remove(sourceAutoPropertyDescriptor);
            }

            if (association.Source.ImplementNotify || association.TargetMultiplicity == Multiplicity.ZeroMany)
            {
               PropertyDescriptor targetAutoPropertyDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().SingleOrDefault(x => x.Name == "TargetAutoProperty");
               if (targetAutoPropertyDescriptor != null) propertyDescriptors.Remove(targetAutoPropertyDescriptor);
            }

            // only display roles for 1..1 and 0-1..0-1 associations
            if (((association.SourceMultiplicity != Multiplicity.One || association.TargetMultiplicity != Multiplicity.One) &&
                 (association.SourceMultiplicity != Multiplicity.ZeroOne || association.TargetMultiplicity != Multiplicity.ZeroOne)))
            {
               PropertyDescriptor sourceRolePropertyDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().SingleOrDefault(x => x.Name == "SourceRole");
               if (sourceRolePropertyDescriptor != null) propertyDescriptors.Remove(sourceRolePropertyDescriptor);

               PropertyDescriptor targetRolePropertyDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().SingleOrDefault(x => x.Name == "TargetRole");
               if (targetRolePropertyDescriptor != null) propertyDescriptors.Remove(targetRolePropertyDescriptor);
            }

            // only display delete behavior on the principal end
            if (association.SourceRole != EndpointRole.Principal)
            {
               PropertyDescriptor sourceDeleteActionPropertyDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().SingleOrDefault(x => x.Name == "SourceDeleteAction");
               if (sourceDeleteActionPropertyDescriptor != null) propertyDescriptors.Remove(sourceDeleteActionPropertyDescriptor);
            }

            if (association.TargetRole != EndpointRole.Principal)
            {
               PropertyDescriptor targetDeleteActionPropertyDescriptor = propertyDescriptors.OfType<PropertyDescriptor>().SingleOrDefault(x => x.Name == "TargetDeleteAction");
               if (targetDeleteActionPropertyDescriptor != null) propertyDescriptors.Remove(targetDeleteActionPropertyDescriptor);
            }

            /********************************************************************************/

            DomainPropertyInfo collectionClassPropertyInfo = storeDomainDataDirectory.GetDomainProperty(Association.CollectionClassDomainPropertyId);
            DomainPropertyInfo isCollectionClassTrackingPropertyInfo = storeDomainDataDirectory.GetDomainProperty(Association.IsCollectionClassTrackingDomainPropertyId);

            // Define attributes for the tracking property/properties so that the Properties window displays them correctly.  
            Attribute[] collectionClassAttributes =
            {
               new DisplayNameAttribute("Collection Class"),
               new DescriptionAttribute("Type of collections generated. Overrides the default collection class for the model"),
               new CategoryAttribute("Code Generation")
            };

            propertyDescriptors.Add(new TrackingPropertyDescriptor(association, collectionClassPropertyInfo, isCollectionClassTrackingPropertyInfo, collectionClassAttributes));
         }

         // Return the property descriptors for this element  
         return propertyDescriptors;
      }

   }
}
