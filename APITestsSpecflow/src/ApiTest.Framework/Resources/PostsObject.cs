using System.Collections.Generic;
using ApiTesting.CSharp.Framework.BasePageObjects;
using ApiTesting.CSharp.Framework.Models;
using NLog;
using RestSharp;

namespace ApiTesting.CSharp.Framework.Resources
{
    public class PostsObject : ResourceObject
    {
        private const string RequestPath = "posts";
        private const string RequestPathWithId = "posts/{id}";

        private new static readonly RestClient RestClient = new RestClient(Settings.Url);
        private new static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public PostsObject() : base(RestClient, Logger)
        {
        }

        public IRestResponse<List<Post>> GetPosts()
        {
            RestRequest request = new RestRequest(RequestPath, Method.GET);
            return ExecuteRequest<List<Post>>(request);
        }

        public IRestResponse<List<Post>> GetPosts(string userId)
        {
            RestRequest request = new RestRequest(RequestPath, Method.GET);
            request.AddParameter("userId", userId);

            return ExecuteRequest<List<Post>>(request);
        }

        public IRestResponse<Post> GetPost(string postId)
        {
            RestRequest request = new RestRequest(RequestPathWithId, Method.GET);
            request.AddUrlSegment("id", postId);

            return ExecuteRequest<Post>(request);
        }

        public IRestResponse<Post> SendPost(Post post)
        {
            RestRequest request = new RestRequest(RequestPath, Method.POST);
            request.AddParameter("userId", post.UserId);
            request.AddParameter("title", post.Title);
            request.AddParameter("body", post.Body);

            return ExecuteRequest<Post>(request);
        }


        public IRestResponse<Post> UpdatePost(Post post)
        {
            RestRequest request = new RestRequest(RequestPathWithId, Method.PUT);
            request.AddUrlSegment("id", post.Id.ToString());
            request.AddParameter("userId", post.UserId);
            request.AddParameter("title", post.Title);
            request.AddParameter("body", post.Body);

            return ExecuteRequest<Post>(request);
        }

        public IRestResponse DeletePost(Post post)
        {
            RestRequest request = new RestRequest(RequestPathWithId, Method.DELETE);
            request.AddUrlSegment("id", post.Id.ToString());

            return ExecuteRequest(request);
        }
    }
}
