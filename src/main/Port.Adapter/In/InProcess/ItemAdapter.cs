using CQRSlite.Commands;
using ei8.Data.Tag.Application;
using System;
using System.Threading.Tasks;

namespace ei8.Data.Tag.Port.Adapter.In.InProcess
{
    public class ItemAdapter : IItemAdapter
    {
        private readonly ICommandSender commandSender;

        public ItemAdapter(ICommandSender commandSender)
        {
            this.commandSender = commandSender;
        }

        public async Task ChangeTag(Guid id, string newTag, Guid authorId, int expectedVersion)
        {
            await this.commandSender.Send(new ChangeTag(id, newTag, authorId, expectedVersion));
        }
    }
}
