import {ActivatedRouteSnapshot, DetachedRouteHandle, RouteReuseStrategy} from "@angular/router";

export class RouteReuse implements RouteReuseStrategy
{
  handlers: { [key: string]: DetachedRouteHandle } = {};

  calcKey (route: ActivatedRouteSnapshot)
  {
    let next = route;
    let url = "";
    while (next)
    {
      if (next.url)
      {
        url = next.url.join('/');
      }
      next = next.firstChild;
    }
    // console.log('url', url);
    return url;
  }

  shouldDetach (route: ActivatedRouteSnapshot): boolean
  {
    let shouldReuse = false;
    if (route.routeConfig.data)
    {
      route.routeConfig.data.reuse ? shouldReuse = true : shouldReuse = false;
    }

    if (shouldReuse) console.log('reusing this route', route);

    return shouldReuse;
  }

  store (route: ActivatedRouteSnapshot, handle: DetachedRouteHandle): void
  {
    // console.log('CustomReuseStrategy:store', route, handle);
    this.handlers[this.calcKey(route)] = handle;

  }

  shouldAttach (route: ActivatedRouteSnapshot): boolean
  {
    // console.log('CustomReuseStrategy:shouldAttach', route);
    return !!route.routeConfig && !!this.handlers[this.calcKey(route)];
  }

  retrieve (route: ActivatedRouteSnapshot): DetachedRouteHandle
  {
    // console.log('CustomReuseStrategy:retrieve', route);
    if (!route.routeConfig) return null;
    return this.handlers[this.calcKey(route)];
  }

  shouldReuseRoute (future: ActivatedRouteSnapshot, curr: ActivatedRouteSnapshot): boolean
  {
    // console.log('CustomReuseStrategy:shouldReuseRoute', future, curr);
    return this.calcKey(curr) === this.calcKey(future);
  }
}
