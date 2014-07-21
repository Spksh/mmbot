﻿using System.Globalization;
using ServiceStack;
using ServiceStack.Text;

namespace MMBot.HipChat
{
    class HipChatAPI
    {
        private const string GetAllRoomsEndpoint = "https://api.hipchat.com/v2/room";
        private const string GetRoomEndpoint = "https://api.hipchat.com/v2/room/{0}";
        private const string ViewUserEndpoint = "https://api.hipchat.com/v2/user/{0}";

        private readonly string authToken;

        public HipChatAPI(string authToken)
        {
            this.authToken = authToken;
        }

        private JsConfigScope GetJsConfigScope()
        {
            return JsConfig.With(
                emitLowercaseUnderscoreNames: true,
                emitCamelCaseNames: false,
                propertyConvention: PropertyConvention.Lenient);
        }

        public HipchatGetAllRoomsResponse GetAllRooms(int startIndex = 0, int maxResults = 100, bool includeArchived = false)
        {
            using (GetJsConfigScope())
            {
                return GetAllRoomsEndpoint
                    .AddQueryParam("auth_token", authToken)
                    .AddQueryParam("start-index", startIndex)
                    .AddQueryParam("max-results", maxResults)
                    .AddQueryParam("include-archived", includeArchived)
                    .GetJsonFromUrl()
                    .FromJson<HipchatGetAllRoomsResponse>();
            }
        }

        public HipchatGetRoomResponse GetRoom(int roomId)
        {
            using (GetJsConfigScope())
            {
                return GetRoomEndpoint.Fmt(roomId.ToString(CultureInfo.InvariantCulture))
                    .AddQueryParam("auth_token", authToken)
                    .GetJsonFromUrl()
                    .FromJson<HipchatGetRoomResponse>();
            }
        }

        public HipchatViewUserResponse ViewUser(string userId)
        {
            using (GetJsConfigScope())
            {
                return ViewUserEndpoint.Fmt(userId)
                    .AddQueryParam("auth_token", authToken)
                    .GetJsonFromUrl()
                    .FromJson<HipchatViewUserResponse>();
            }
        }
    }
}