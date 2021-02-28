import {ActivatedRouteSnapshot, DetachedRouteHandle, RouteReuseStrategy} from "@angular/router";

export class RouteReuse implements RouteReuseStrategy
{
  storedRouteHandles = new Map<string, DetachedRouteHandle>();
  retrievable = {
    'search': true
  };

  // Gets the route from the hashmap
  retrieve (route: ActivatedRouteSnapshot): DetachedRouteHandle | null
  {
    console.log('retrieving route', route);
    return this.storedRouteHandles.get(this.getPath(route)) as DetachedRouteHandle;
  }

  // Should it create a new component from scratch or reuse
  shouldAttach (route: ActivatedRouteSnapshot): boolean
  {
    if (this.retrievable[this.getPath(route)])
    {
      return this.storedRouteHandles.has(this.getPath(route));
    }
    return false;
  }

  // Should store the outgoing component / route
  shouldDetach (route: ActivatedRouteSnapshot): boolean
  {
    const path = this.getPath(route);
    return this.retrievable.hasOwnProperty(path);
  }

  // If the routes are different then trigger router reuse
  shouldReuseRoute (future: ActivatedRouteSnapshot, curr: ActivatedRouteSnapshot): boolean
  {
    if (this.getPath(future) === 'event/:id' && this.getPath(curr) === 'search')
    {
      this.retrievable['search'] = true;
    } else
    {
      this.retrievable['search'] = false;
    }

    return future.routeConfig === curr.routeConfig;
  }

  // stores the route in the hashmap
  store (route: ActivatedRouteSnapshot, handle: DetachedRouteHandle | null): void
  {
    console.log('storing route', route);
    this.storedRouteHandles.set(this.getPath(route), handle);
  }

  private getPath (route: ActivatedRouteSnapshot): string
  {
    if (route.routeConfig !== null && route.routeConfig.path !== null)
    {
      console.log(route.routeConfig.path);
      return route.routeConfig.path;
    }
    return '';
  }
}

