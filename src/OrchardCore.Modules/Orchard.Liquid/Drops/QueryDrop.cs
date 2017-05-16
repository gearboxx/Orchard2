﻿using System;
using System.Linq;
using DotLiquid;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using YesSql;

namespace Orchard.Liquid.Drops
{
    public class QueryDrop : Drop, IIndexable
    {
        private readonly IContentManager _contentManager;
        private readonly ISession _session;

        public QueryDrop(IContentManager contentManager, ISession session)
        {
            _contentManager = contentManager;
            _session = session;
            Query = _session.QueryAsync<ContentItem, ContentItemIndex>().Where(x => x.Published);
        }

        public ContentTypeDrop ContentType => new ContentTypeDrop(this);
        public OrderByDrop OrderBy => new OrderByDrop(this);
        public OrderByDescendingDrop OrderByDescending => new OrderByDescendingDrop(this);
        public WhereDrop Where => new WhereDrop(this);
        public SkipDrop Skip => new SkipDrop(this);
        public TakeDrop Take => new TakeDrop(this);
        
        public IQuery<ContentItem, ContentItemIndex> Query { get; set; }

        public object List
        {
            get
            {
                var result = Query.List().GetAwaiter().GetResult().ToList();
                return result;
            }
        }
    }

    public class ContentTypeDrop : Drop, IIndexable
    {
        private QueryDrop _query;

        public ContentTypeDrop(QueryDrop query)
        {
            _query = query;
        }

        object IIndexable.this[object key]
        {
            get
            {
                _query.Query = _query.Query.Where(x => x.ContentType == key.ToString());
                return _query;
            }
        }

        bool IIndexable.ContainsKey(object key)
        {
            return true;
        }
    }

    public class OrderByDrop : Drop, IIndexable
    {
        private QueryDrop _query;

        public OrderByDrop(QueryDrop query)
        {
            _query = query;
        }

        object IIndexable.this[object key]
        {
            get
            {
                switch (Convert.ToString(key))
                {
                    case "Created": _query.Query = _query.Query.OrderBy(x => x.CreatedUtc); break;
                    case "Modified": _query.Query = _query.Query.OrderBy(x => x.ModifiedUtc); break;
                    case "Published": _query.Query = _query.Query.OrderBy(x => x.PublishedUtc); break;
                    case "Id": _query.Query = _query.Query.OrderBy(x => x.Id); break;
                    case "ContentItemId": _query.Query = _query.Query.OrderBy(x => x.ContentItemId); break;
                }
                return _query;
            }
        }

        bool IIndexable.ContainsKey(object key)
        {
            return true;
        }
    }

    public class OrderByDescendingDrop : Drop, IIndexable
    {
        private QueryDrop _query;

        public OrderByDescendingDrop(QueryDrop query)
        {
            _query = query;
        }

        object IIndexable.this[object key]
        {
            get
            {
                switch (Convert.ToString(key))
                {
                    case "Created": _query.Query = _query.Query.OrderByDescending(x => x.CreatedUtc); break;
                    case "Modified": _query.Query = _query.Query.OrderByDescending(x => x.ModifiedUtc); break;
                    case "Published": _query.Query = _query.Query.OrderByDescending(x => x.PublishedUtc); break;
                    case "Id": _query.Query = _query.Query.OrderByDescending(x => x.Id); break;
                    case "ContentItemId": _query.Query = _query.Query.OrderByDescending(x => x.ContentItemId); break;
                }
                return _query;
            }
        }

        bool IIndexable.ContainsKey(object key)
        {
            return true;
        }
    }

    public class WhereDrop : Drop, IIndexable
    {
        private QueryDrop _query;

        public WhereDrop(QueryDrop query)
        {
            _query = query;
        }

        // TODO: Secure
        object IIndexable.this[object key]
        {
            get
            {
                _query.Query = _query.Query.Where(Convert.ToString(key));
                return _query;
            }
        }

        bool IIndexable.ContainsKey(object key)
        {
            return true;
        }
    }

    public class TakeDrop : Drop, IIndexable
    {
        private QueryDrop _query;

        public TakeDrop(QueryDrop query)
        {
            _query = query;
        }

        object IIndexable.this[object key]
        {
            get
            {
                _query.Query = _query.Query.Take(Convert.ToInt32(key)).With<ContentItemIndex>();
                return _query;
            }
        }

        bool IIndexable.ContainsKey(object key)
        {
            return true;
        }
    }

    public class SkipDrop : Drop, IIndexable
    {
        private QueryDrop _query;

        public SkipDrop(QueryDrop query)
        {
            _query = query;
        }

        object IIndexable.this[object key]
        {
            get
            {
                _query.Query = _query.Query.Skip(Convert.ToInt32(key)).With<ContentItemIndex>();
                return _query;
            }
        }

        bool IIndexable.ContainsKey(object key)
        {
            return true;
        }
    }
}