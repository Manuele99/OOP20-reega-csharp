using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reega.Navigation
{
    public sealed class DataTemplateManager
    {
        private static DataTemplateManager _instance;
        private static readonly object _lock = new();

        private readonly ConcurrentDictionary<Type, IDataTemplate> _templates = new();

        private DataTemplateManager() { }

        /// <summary>
        /// Static instance
        /// </summary>
        public static DataTemplateManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance is null)
                        _instance = new DataTemplateManager();

                    return _instance;
                }
            }
        }

        /// <summary>
        /// Add a data template
        /// </summary>
        /// <param name="template">Template to add</param>
        public void AddTemplate(IDataTemplate template)
        {
            _ = template ?? throw new ArgumentNullException(nameof(template));
            this._templates.TryAdd(template.GetDataObjectClass(), template);
        }

        /// <summary>
        /// Remove a template
        /// </summary>
        /// <param name="template">Template to remove</param>
        public void RemoveTemplate(IDataTemplate template)
        {
            _ = template ?? throw new ArgumentNullException(nameof(template));
            this._templates.TryRemove(template.GetDataObjectClass(), out _);
        }

        /// <summary>
        /// Remove a template based on its <see cref="IDataTemplate.GetDataObjectClass"/>
        /// </summary>
        /// <param name="dataObjectType">Data object class to remove</param>
        public void RemoveTemplate(Type dataObjectType)
        {
            _ = dataObjectType ?? throw new ArgumentNullException(nameof(dataObjectType));
            this._templates.TryRemove(dataObjectType, out _);
        }

        /// <summary>
        /// Get the template associated with <paramref name="dataObjectType"/>
        /// </summary>
        /// <param name="dataObjectType">Data object class to search</param>
        /// <returns>A non-null <see cref="IDataTemplate"/> if it has been found, a null <see cref="IDataTemplate"/> otherwise</returns>
#nullable enable
        public IDataTemplate? GetTemplate(Type dataObjectType)
        {
            _ = dataObjectType ?? throw new ArgumentNullException(nameof(dataObjectType));
            this._templates.TryGetValue(dataObjectType, out IDataTemplate? value);
            return value;
        }
#nullable disable

    }
}
