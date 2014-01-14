﻿using System;
using System.Collections.Generic;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Owin;

namespace MMBot.Router.Nancy
{
    public class NancyRouter : IRouter
    {
        private int _port;
        private IDisposable _webappDisposable;
        private Robot _robot;
        private bool _isConfigured;
        private readonly IDictionary<Route, Func<OwinContext, object>> _routes = new Dictionary<Route, Func<OwinContext, object>>();

        public virtual IDictionary<Route, Func<OwinContext, object>> Routes
        {
            get { return _routes; }
        }

        public virtual void Configure(Robot robot, int port)
        {
            _robot = robot;
            _port = port;
            _isConfigured = true;
        }

        public virtual void Start()
        {
            if (!_isConfigured)
            {
                throw new InvalidOperationException("The router has not yet been configured. You must call Configure before calling start");
            }

            var url = string.Format("http://+:{0}", _port);
            _webappDisposable = WebApp.Start(url, app => app.UseNancy(options => options.Bootstrapper = new Bootstrapper(this)));
            
            _robot.Logger.Info(string.Format("Router (Nancy) is running on http://localhost:{0}", _port));
        }

        public virtual void Stop()
        {
            if (_webappDisposable == null)
            {
                return;
            }
            
            _webappDisposable.Dispose();
            _webappDisposable = null;
        }

        public virtual void Get(string path, Func<OwinContext, object> actionFunc)
        {
            Routes.Add(new Route{ Method = Route.RouteMethod.Get, Path = path}, actionFunc);
        }

        public virtual void Get(string path, Action<OwinContext> action)
        {
            Routes.Add(new Route { Method = Route.RouteMethod.Get, Path = path }, context => { action(context);
                                                                                                 return null;
            } );
        }

        public virtual void Post(string path, Func<OwinContext, object> actionFunc)
        {
            Routes.Add(new Route { Method = Route.RouteMethod.Post, Path = path }, actionFunc);
        }


        public virtual void Post(string path, Action<OwinContext> action)
        {
            Routes.Add(new Route { Method = Route.RouteMethod.Post, Path = path }, context =>
            {
                action(context);
                return null;
            });
        }
    }
}