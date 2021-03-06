﻿using System;
using System.Collections.Generic;
using System.Security.Principal;
using N2.Definitions;
using N2.Definitions.Static;
using N2.Edit;
using N2.Engine;
using N2.Persistence;

namespace N2.Management.Content.Templates
{
	[Service]
	public class ContentTemplateRepository
	{
		public const string TemplateDescription = "TemplateDescription";

		IPersister persister;
		DefinitionMap map;
		ContainerRepository<TemplateContainer> container;

		public ContentTemplateRepository(IPersister persister, DefinitionMap map, ContainerRepository<TemplateContainer> container)
		{
			this.persister = persister;
			this.map = map;
			this.container = container;
		}

		#region ITemplateRepository Members

		public TemplateDefinition GetTemplate(string templateKey)
		{
			TemplateContainer templates = container.GetBelowRoot();
			if (templates == null)
				return null;

			var template = templates.GetChild(templateKey);
			return CreateTemplateInfo(template);
		}

		private TemplateDefinition CreateTemplateInfo(ContentItem template)
		{
			var info = new TemplateDefinition
			{
				Name = template.Name,
				Title = template.Title,
				Description = template.GetDetail(TemplateDescription, ""),
				TemplateUrl = template.Url,
				Definition = map.GetOrCreateDefinition(template.GetContentType(), template.Name),
				Template = () =>
				{
					var clone = template.Clone(true);
					clone.SetDetail(TemplateDescription, null, typeof(string));
					clone.Title = "";
					clone.Name = null;
					clone.TemplateKey = template.Name;
					return clone;
				},
				Original = () => template
			};
			return info;
		}

		public IEnumerable<TemplateDefinition> GetAllTemplates()
		{
			TemplateContainer templates = container.GetBelowRoot();
			if (templates == null)
				yield break;

			foreach (ContentItem child in templates.Children)
			{
				yield return CreateTemplateInfo(child);
			}
		}

		public IEnumerable<TemplateDefinition> GetTemplates(Type contentType, IPrincipal user)
		{
			foreach(var template in GetAllTemplates())
			{
				if (template.Definition.ItemType != contentType)
					continue;
				if (!template.Template().IsAuthorized(user))
					continue;

				yield return template;
			}
		}

		public void AddTemplate(ContentItem templateItem)
		{
			TemplateContainer templates = container.GetOrCreateBelowRoot((c) =>
				{
					c.Title = "Templates";
					c.Name = "Templates";
					c.Visible = false;
					c.SortOrder = int.MaxValue - 1001000;
				});

			templateItem.Name = null;
			templateItem.AddTo(templates);
			persister.Save(templateItem);
		}

		public void RemoveTemplate(string templateKey)
		{
			TemplateContainer templates = container.GetBelowRoot();
			if (templates == null)
				return;

			ContentItem template = templates.GetChild(templateKey);
			if (template == null)
				return;

			persister.Delete(template);			
		}

		#endregion
	}
}
