using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grain.Tests.Models.TestModels
{
    public partial class Entity : IEntity
    {
        public int ID { get; set; }

        public string DisplayName { get; set; }

        public string Contents { get; set; }

        public int CategoriesID { get; set; }

        public int? SubCategoriesID { get; set; }

        public int UniversitiesID { get; set; }

        public int? OwnerId { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsGraded { get; set; }

        public int PresentationTemplate { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public DateTime? DeleteDate { get; set; }

        public DateTime? HiddenDate { get; set; }

        public DateTime? LockDate { get; set; }

        public DateTime? TestInProgressDate { get; set; }

        public int? TestInProgressDuration { get; set; }
    }
}
