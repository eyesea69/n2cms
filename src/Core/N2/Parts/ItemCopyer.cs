using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using N2.Persistence;
using N2.Web;

namespace N2.Parts
{
	public class ItemMover : PartsAjaxService
	{
		private readonly IPersister persister;

		public ItemMover(IPersister persister, AjaxRequestDispatcher dispatcher)
			: base(dispatcher)
		{
			this.persister = persister;
		}

		public override string Name
		{
			get { return "move"; }
		}

		public override NameValueCollection Handle(NameValueCollection request)
		{
			MoveItem(request);
			return new NameValueCollection();
		}

		private void MoveItem(NameValueCollection request)
		{
			ContentItem item = persister.Get(int.Parse(request["item"]));
			ContentItem parent;

			item.ZoneName = request["zone"];

			string before = request["before"];
			string below = request["below"];
			if (!string.IsNullOrEmpty(below))
			{
				parent = persister.Get(int.Parse(below));
				Utility.Insert(item, parent, parent.Children.Count);
			}
			else
			{
				ContentItem beforeItem = persister.Get(int.Parse(before));
				parent = beforeItem.Parent;
				int newIndex = parent.Children.IndexOf(beforeItem);
				Utility.Insert(item, parent, newIndex);
			}
			IEnumerable<ContentItem> changedItems = Utility.UpdateSortOrder(parent.Children);
			foreach (ContentItem changedItem in changedItems)
				persister.Save(changedItem);
			persister.Save(item);
		}
	}
}