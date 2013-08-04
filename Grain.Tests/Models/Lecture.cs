using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Grain.Serialization;
using Omu.ValueInjecter;

namespace Grain.Tests.Models.TestModels
{   
    /// <summary>
    ///
    /// </summary>
    //[MetadataType(typeof(LectureObject.Metadata))]
    public partial class Lecture : Entity, IEntity
    {
        public Lecture() 
        {
            this.LectureDefinition = new LectureDefinition();
            Init();
        }

        public LectureDefinition LectureDefinition { get; private set; }

        public void Init() 
        {
            if (!String.IsNullOrWhiteSpace(Contents))
                LectureDefinition = Contents.FromXml<LectureDefinition>();        
        }

        public int ProviderId { get { return LectureDefinition.ProviderId; } set { LectureDefinition.ProviderId = value; } }

        public string Title { get { return LectureDefinition.Title; } set { LectureDefinition.Title = value; } }

        public string Description { get { return LectureDefinition.Description; } set { LectureDefinition.Description = value; } }

        public object LectureContent { get { return LectureDefinition.LectureContent; } set { LectureDefinition.LectureContent = value; } }

        public string URL { get { return LectureDefinition.URL; } set { LectureDefinition.URL = value; } }

        public TimeSpan Duration { get { return LectureDefinition.Duration; } set { LectureDefinition.Duration = value; } }

        public bool AvailableAfterwards { get { return LectureDefinition.AvailableAfterwards; } set { LectureDefinition.AvailableAfterwards = value; } }

        //internal class Metadata
        //{
        //    [Display(Name = "Parent Object", AutoGenerateField = false)]
        //    public int ObjectToObjectMaps;
        //    [Display(Name = "Children Objects")]
        //    public int ObjectToObjectMaps1;

        //    [UIHint("Text")]
        //    public string Title;

        //    [UIHint("Text")]
        //    public string Description;

        //    [UIHint("Text")]
        //    public string URL;

        //    [UIHint("Text")]
        //    public string Duration;

        //    [UIHint("Text")]
        //    public string AvailableAfterwards;

        //    [UIHint("Text")]
        //    public string ProviderId;

        //    //[Display(AutoGenerateField = false)]
        //    //public string Contents;
        //}
    }

    /// <summary>
    /// The Object Definition is used to serialize and deserialize the IObject.Contents value
    /// </summary>
    [DataContract]
    public partial class LectureDefinition
    {
        [DataMember(Order = 1)]
        public int ProviderId { get; set; }

        [DataMember(Order = 5)]
        public string Title { get; set; }

        [DataMember(Order = 6)]
        public string Description { get; set; }

        [DataMember(Order = 10)]
        public object LectureContent { get; set; }

        [DataMember(Order = 15)]
        public string URL { get; set; }

        [DataMember(Order = 20)]
        public TimeSpan Duration { get; set; }

        [DataMember(Order = 21)]
        public bool AvailableAfterwards { get; set; }
    }

    public static partial class LectureExtensions 
    { 
        /// <summary>
        /// Prepares a LectureObject to be put into the database, through a specific, serialized cast to an Entity
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Entity ToEntity(this Lecture obj)
        {
            Entity _result = obj as Entity;
            _result.Contents = obj.LectureDefinition.ToXml();

            return _result;
        }

        /// <summary>
        /// Prepares a Collection of LectureObjects to be put into the database, through a specific, serialized cast to a collection of Entities
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IEnumerable<Entity> ToEntities(this IEnumerable<Lecture> objects) 
        {
            List<Entity> _entities = new List<Entity> { };

            foreach (var lecture in objects) 
            {
                _entities.Add(lecture.ToEntity());
            }

            return _entities;
        }

        /// <summary>
        /// Gets a Lecture object from an Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Lecture ToLecture(this Entity entity) 
        {
            Lecture _lectureFromEntity = new Lecture();
            _lectureFromEntity.InjectFrom(entity);
            _lectureFromEntity.Init();
            return _lectureFromEntity;
        }
        
        /// <summary>
        /// Gets a collection of Lecures from a collection of Entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static IEnumerable<Lecture> ToLectures(this IEnumerable<Entity> entities) 
        {
            List<Lecture> _lectures = new List<Lecture> { };

            foreach (var lecture in entities)
            {
                _lectures.Add(lecture.ToLecture());
            }

            return _lectures;
        }
    }
}