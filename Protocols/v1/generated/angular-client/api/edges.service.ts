/**
 * PolarisApi
 * This is the api for Polaris Data Analysis Project on  [PolarisGithub](https://github.com/Star-Academy/StarAcademy-Group2/) 
 *
 * OpenAPI spec version: v1
 * 
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 *//* tslint:disable:no-unused-variable member-ordering */

import { Inject, Injectable, Optional }                      from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams,
         HttpResponse, HttpEvent }                           from '@angular/common/http';
import { CustomHttpUrlEncodingCodec }                        from '../encoder';

import { Observable }                                        from 'rxjs';

import { Edge } from '../model/edge';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';


@Injectable()
export class EdgesService {

    protected basePath = 'http://localhost:5000/api/v1';
    public defaultHeaders = new HttpHeaders();
    public configuration = new Configuration();

    constructor(protected httpClient: HttpClient, @Optional()@Inject(BASE_PATH) basePath: string, @Optional() configuration: Configuration) {
        if (basePath) {
            this.basePath = basePath;
        }
        if (configuration) {
            this.configuration = configuration;
            this.basePath = basePath || configuration.basePath || this.basePath;
        }
    }

    /**
     * @param consumes string[] mime-types
     * @return true: consumes contains 'multipart/form-data', false: otherwise
     */
    private canConsumeForm(consumes: string[]): boolean {
        const form = 'multipart/form-data';
        for (const consume of consumes) {
            if (form === consume) {
                return true;
            }
        }
        return false;
    }


    /**
     * Add a new Edge to the dataset
     * 
     * @param body Edge object to be sent
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public addEdge(body: Edge, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public addEdge(body: Edge, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public addEdge(body: Edge, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public addEdge(body: Edge, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (body === null || body === undefined) {
            throw new Error('Required parameter body was null or undefined when calling addEdge.');
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
            'application/json'
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.request<any>('post',`${this.basePath}/edges`,
            {
                body: body,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Delete Edges by id
     * 
     * @param edgeId Edge id to delete
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public deleteEdge(edgeId: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public deleteEdge(edgeId: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public deleteEdge(edgeId: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public deleteEdge(edgeId: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (edgeId === null || edgeId === undefined) {
            throw new Error('Required parameter edgeId was null or undefined when calling deleteEdge.');
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.request<any>('delete',`${this.basePath}/edges/${encodeURIComponent(String(edgeId))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Get Edges by Id
     * Returns a single Edge
     * @param edgeId Id of Edge to return
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public getEdgeById(edgeId: string, observe?: 'body', reportProgress?: boolean): Observable<Edge>;
    public getEdgeById(edgeId: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Edge>>;
    public getEdgeById(edgeId: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Edge>>;
    public getEdgeById(edgeId: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (edgeId === null || edgeId === undefined) {
            throw new Error('Required parameter edgeId was null or undefined when calling getEdgeById.');
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'application/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.request<Edge>('get',`${this.basePath}/edges/${encodeURIComponent(String(edgeId))}`,
            {
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Get Edge(s)
     * 
     * @param filter Filter on Edges in [\&quot;{field} {operator} {value}\&quot;, ...] format
     * @param pageIndex Pagination pageIndex
     * @param pageSize Pagination pageSize
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public getEdges(filter?: Array<string>, pageIndex?: number, pageSize?: number, observe?: 'body', reportProgress?: boolean): Observable<Array<Edge>>;
    public getEdges(filter?: Array<string>, pageIndex?: number, pageSize?: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<Edge>>>;
    public getEdges(filter?: Array<string>, pageIndex?: number, pageSize?: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<Edge>>>;
    public getEdges(filter?: Array<string>, pageIndex?: number, pageSize?: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {




        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (filter) {
            filter.forEach((element) => {
                queryParameters = queryParameters.append('filter', <any>element);
            })
        }
        if (pageIndex !== undefined && pageIndex !== null) {
            queryParameters = queryParameters.set('pageIndex', <any>pageIndex);
        }
        if (pageSize !== undefined && pageSize !== null) {
            queryParameters = queryParameters.set('pageSize', <any>pageSize);
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'application/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.request<Array<Edge>>('get',`${this.basePath}/edges`,
            {
                params: queryParameters,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * Update an existing Edge
     * 
     * @param body Edge object to be sent
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public updateEdge(body: Edge, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public updateEdge(body: Edge, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public updateEdge(body: Edge, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public updateEdge(body: Edge, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (body === null || body === undefined) {
            throw new Error('Required parameter body was null or undefined when calling updateEdge.');
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
            'application/json'
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.request<any>('put',`${this.basePath}/edges`,
            {
                body: body,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

}
