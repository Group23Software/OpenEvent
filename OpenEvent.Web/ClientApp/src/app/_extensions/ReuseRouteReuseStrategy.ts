import {ActivatedRouteSnapshot, DetachedRouteHandle, RouteReuseStrategy} from "@angular/router";
import {ComponentRef} from "@angular/core";

export class ReuseRouteReuseStrategy implements RouteReuseStrategy
{
    private handlers: { [key: string]: DetachedRouteHandle } = {};

    shouldDetach (route: ActivatedRouteSnapshot): boolean
    {
        if (!route.routeConfig || route.routeConfig.loadChildren)
        {
            return false;
        }
        /** Whether this route should be re used or not */
        let shouldReuse = false;
        if (
            route.routeConfig.data &&
            route.routeConfig.data.reuseRoute &&
            typeof route.routeConfig.data.reuseRoute === 'boolean'
        )
        {
            shouldReuse = route.routeConfig.data.reuseRoute;
        }
        return shouldReuse;
    }

    store (route: ActivatedRouteSnapshot, handler: DetachedRouteHandle): void
    {
        if (handler) this.handlers[this.getUrl(route)] = handler;
    }

    shouldAttach (route: ActivatedRouteSnapshot): boolean
    {
        return !!this.handlers[this.getUrl(route)];
    }

    retrieve (route: ActivatedRouteSnapshot): DetachedRouteHandle
    {
        if (!route.routeConfig || route.routeConfig.loadChildren)
        {
            return null;
        }
        return this.handlers[this.getUrl(route)];
    }

    shouldReuseRoute (
        future: ActivatedRouteSnapshot,
        current: ActivatedRouteSnapshot
    ): boolean
    {
        /** We only want to reuse the route if the data of the route config contains a reuse true boolean */
        let reUseUrl = false;
        if (
            future.routeConfig &&
            future.routeConfig.data &&
            typeof future.routeConfig.data.reuseRoute === 'boolean'
        )
        {
            reUseUrl = future.routeConfig.data.reuseRoute;
        }
        const defaultReuse = future.routeConfig === current.routeConfig;
        return reUseUrl || defaultReuse;
    }

    /**
     * Returns a url for the current route
     * @param route
     */
    private getUrl (route: ActivatedRouteSnapshot): string
    {
        /** The url we are going to return */
        if (route.routeConfig)
        {
            const url = route.routeConfig.path;
            return url;
        }
    }

    /**
     * Clearing / Destorying all handles
     */
    clearHandles ()
    {
        for (const key in this.handlers)
        {
            this.destroyHandle(this.handlers[key]);
        }
        this.handlers = {};
    }

    /**
     * Destroying a handle
     * @param handle
     */
    private destroyHandle (handle: DetachedRouteHandle): void
    {
        const componentRef: ComponentRef<any> = handle['componentRef'];
        if (componentRef) componentRef.destroy();
    }
}
