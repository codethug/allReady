using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AllReady.Areas.Admin.Features.Tasks;
using AllReady.Areas.Admin.Models;
using AllReady.Models;
using AllReady.UnitTest.Features.Campaigns;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AllReady.UnitTest.Areas.Admin.Features.Tasks
{
    public class EditTaskCommandHandlerAsyncTests : InMemoryContextTest
    {
        private Organization _htb;
        private Campaign _firePrev;
        private Event _queenAnne;

        protected override void LoadTestData()
        {
            var context = ServiceProvider.GetService<AllReadyContext>();
            _htb = new Organization()
            {
                Name = "Humanitarian Toolbox",
                LogoUrl = "http://www.htbox.org/upload/home/ht-hero.png",
                WebUrl = "http://www.htbox.org",
                Campaigns = new List<Campaign>()
            };

            _firePrev = new Campaign()
            {
                Name = "Neighborhood Fire Prevention Days",
                ManagingOrganization = _htb,
                CampaignContacts = new List<CampaignContact>()
            };
            
            _htb.Campaigns.Add(_firePrev);
            _queenAnne = new Event()
            {
                Id = 1,
                Name = "Queen Anne Fire Prevention Day",
                Campaign = _firePrev,
                CampaignId = _firePrev.Id,
                StartDateTime = new DateTime(2015, 7, 4, 10, 0, 0).ToUniversalTime(),
                EndDateTime = new DateTime(2015, 12, 31, 15, 0, 0).ToUniversalTime(),
                Location = new Location { Id = 1 },
                RequiredSkills = new List<EventSkill>(),
                Tasks = new List<AllReadyTask>()
            };
            context.Events.Add(_queenAnne);
            context.SaveChanges();
        }

        [Fact]
        public async Task ModelIsCreated()
        {
            var sut = new EditTaskCommandHandlerAsync(Context);
            var actual = await sut.Handle(new EditTaskCommandAsync { Task = new TaskSummaryModel { EventId = _queenAnne.Id, TimeZoneId = "Eastern Standard Time" } });
            Assert.Equal(1, actual);
        }
    }
}