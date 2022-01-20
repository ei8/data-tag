using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ei8.Data.Tag.Port.Adapter.In.InProcess
{
    public interface IItemAdapter
    {
        Task ChangeTag(Guid id, string newTag, Guid authorId, int expectedVersion);
    }
}
