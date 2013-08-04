using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grain.Tests.Models.TestModels
{
    public interface IEntity
    {
        int ID { get; set; }
        
        string DisplayName { get; set; }
        
        string Contents { get; set; }
                
        int CategoriesID { get; set; }
        
        System.Nullable<int> SubCategoriesID { get; set; }
        
        int UniversitiesID { get; set; }
        
        System.Nullable<int> OwnerId { get; set; }
        
        int CreatedBy { get; set; }
        
        System.DateTime CreatedDate { get; set; }
        
        System.Nullable<int> ModifiedBy { get; set; }
        
        System.Nullable<System.DateTime> ModifiedDate { get; set; }
        
        bool IsGraded { get; set; }

        //global::ALE.Data.SQL.Object.DataTemplates DataTemplate;

        int PresentationTemplate { get; set; }

        System.Nullable<System.DateTime> ReleaseDate { get; set; }

        System.Nullable<System.DateTime> DeleteDate { get; set; }

        System.Nullable<System.DateTime> HiddenDate { get; set; }

        System.Nullable<System.DateTime> LockDate { get; set; }

        System.Nullable<System.DateTime> TestInProgressDate { get; set; }

        System.Nullable<int> TestInProgressDuration { get; set; }

        //EntitySet<Role> Roles { get; set; }

        //EntitySet<CalendarEntry> CalendarEtries { get; set; }

        //EntitySet<CalendarEntry> CalendarEntries1 { get; set; }

        //EntitySet<ObjectToObjectMap> ObjectToObjectMaps { get; set; }

        //EntitySet<ObjectToObjectMap> ObjectToObjectMaps1 { get; set; }

        //EntityRef<Category> Category { get; set; }

        //EntityRef<DataTemplate> DataTemplate1 { get; set; }

        //EntityRef<PresentationTemplate> PresentationTemplate1 { get; set; }

        //EntityRef<SubCategory> SubCategory { get; set; }

        //EntityRef<University> University { get; set; }

        //EntityRef<User> User { get; set; }

        //EntityRef<User> User1 { get; set; }

        //EntityRef<User> User2 { get; set; }
    }
}
