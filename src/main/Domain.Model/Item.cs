using neurUL.Common.CqrsLite;
using neurUL.Common.Domain.Model;
using System;

namespace ei8.Data.Tag.Domain.Model
{
    /// <summary>
    /// Represents an Item.
    /// </summary>
    public class Item : AssertiveAggregateRoot
    {
        private Item() { }

        /// <summary>
        /// Constructs an Item.
        /// </summary>
        /// <param name="id"></param>
        public Item(Guid id, string tag)
        {
            AssertionConcern.AssertArgumentValid(i => i != Guid.Empty, id, Messages.Exception.IdEmpty, nameof(id));
            AssertionConcern.AssertArgumentNotNull(tag, nameof(tag));

            this.Id = id;
            this.ApplyChange(new TagChanged(id, tag));
        }
        
        public string Tag { get; private set; }

        public void ChangeTag(string newTag)
        {
            AssertionConcern.AssertArgumentNotNull(newTag, nameof(newTag));

            if (newTag != this.Tag)
                base.ApplyChange(new TagChanged(this.Id, newTag));
        }

        private void Apply(TagChanged e)
        {
            this.Tag = e.Tag;
        }
    }
}
