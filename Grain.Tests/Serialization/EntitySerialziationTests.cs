using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Grain.Tests.Models;
using Grain.Serialization;
using Grain.Tests.Models.TestModels;

namespace Grain.Tests.Serialization
{
    [TestClass]
    public class EntitySerialziationTests
    {
        #region Setup and TearDown

        Entity _mockEntity;
        Lecture _mockLecture;
        LectureDefinition _mockLectureDefinition;

        [TestInitialize]
        public void Setup() 
        {
            string _lectureXml = @"
<LectureDefinition xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/Grain.Tests.Models.TestModels"">
  <ProviderId>8</ProviderId>
  <Title>The best lecture ever</Title>
  <Description>A lesson in humility</Description>
  <LectureContent xmlns:d2p1=""http://www.w3.org/2001/XMLSchema"" i:type=""d2p1:string"">something</LectureContent>
  <URL>http://google.com</URL>
  <Duration>PT3H</Duration>
  <AvailableAfterwards>false</AvailableAfterwards>
</LectureDefinition>
";

            _mockLectureDefinition = new LectureDefinition
            {
                ProviderId = 8,
                Title = "The best lecture ever",
                Description = "A lesson in humility",
                LectureContent = "something",
                URL = "http://google.com",
                Duration = new TimeSpan(3, 0, 0),
                AvailableAfterwards = false
            };

            _mockLecture = new Lecture
            {
                ID = 1,
                DisplayName = "Test Entity",
                Contents = _lectureXml,
                CategoriesID = 5,
                SubCategoriesID = 25,
                UniversitiesID = 1,
                OwnerId = 256,
                CreatedBy = 256,
                ModifiedBy = null,
                ModifiedDate = null,
                IsGraded = false,
                PresentationTemplate = 7,
                ReleaseDate = new DateTime(2013, 01, 18),
                DeleteDate = null,
                HiddenDate = null,
                LockDate = null,
                TestInProgressDate = null,
                TestInProgressDuration = null,
                // Lecture specific properties follow
                ProviderId = _mockLectureDefinition.ProviderId,
                Title = _mockLectureDefinition.Title,
                Description = _mockLectureDefinition.Description,
                LectureContent = _mockLectureDefinition.LectureContent,
                URL = _mockLectureDefinition.URL,
                Duration = _mockLectureDefinition.Duration,
                AvailableAfterwards = _mockLectureDefinition.AvailableAfterwards
            };

            _mockEntity = new Entity
            {
                ID = 1,
                DisplayName = "Test Entity",
                Contents = _lectureXml,
                CategoriesID = 5,
                SubCategoriesID = 25,
                UniversitiesID = 1,
                OwnerId = 256,
                CreatedBy = 256,
                ModifiedBy = null,
                ModifiedDate = null,
                IsGraded = false,
                PresentationTemplate = 7,
                ReleaseDate = new DateTime(2013, 01, 18),
                DeleteDate = null,
                HiddenDate = null,
                LockDate = null,
                TestInProgressDate = null,
                TestInProgressDuration = null
            };
        }

        [TestCleanup]
        public void TearDown() 
        { 
        
        }

        #endregion Setup and TearDown

        /// <summary>
        /// Potential use cases:
        /// 
        /// db.Objects.Add(_lecture.ToEntity());
        ///
        /// var _lectures = db.Objects.FirstOrDefault(l => l.ID == 3).ToLecture();
        /// </summary>
        [TestMethod]
        [TestCategory("Grain.Serialization")]
        public void LectureSerializationTest()
        {
            Lecture _lecture = _mockLecture;
            Entity _entity = _mockLecture.ToEntity();

            Assert.AreEqual(_lecture.CreatedBy, _entity.CreatedBy);
            Assert.IsNotNull(_entity.Contents);

            Lecture _lectureFromEntity = _mockEntity.ToLecture();
            Assert.IsNotNull(_lectureFromEntity.CreatedBy);
            Assert.IsNotNull(_lectureFromEntity.Title);
            Assert.IsNotNull(_lectureFromEntity.Contents);
        }
    }
}
