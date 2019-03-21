import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AlertifyService } from './alertify.service';
import { PaginationResult } from '../_models/pagination';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getUsers(page? , itemsPerPage?, userParam?, likeParam?): Observable<PaginationResult<User[]>> {
    const paginationResult: PaginationResult<User[]> = new PaginationResult<User[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
        params = params.append('pageNumber', page);
        params = params.append('pageSize', itemsPerPage);
    }

    if (userParam != null) {
      params = params.append('minAge', userParam.minAge);
      params = params.append('maxAge', userParam.maxAge);
      params = params.append('gender', userParam.gender);
      params = params.append('orderBy', userParam.orderBy);
    }

    if (likeParam === 'Likers') {
      params = params.append('likers', 'true');
    }

    if (likeParam === 'Likees') {
      params = params.append('likees', 'true');
    }

    return this.http.get<User[]>(this.baseUrl + 'users', {observe: 'response', params})
    .pipe(
       map (response => {
         paginationResult.result = response.body;
         if (response.headers.get('Pagination') != null) {
            paginationResult.pagination = JSON.parse(response.headers.get('Pagination'));
         }

         return paginationResult;
       })
    );
  }

  getUser(id): Observable<User> {
    return this.http.get<User> (this.baseUrl + 'users/' + id);
  }

  updateUser(id: number, user: User) {
    return this.http.put(this.baseUrl + 'users/' + id, user);
  }

  setMainPhoto(id: number, photoId: number) {
    return this.http.post(this.baseUrl + 'users/' + id + '/profile/' + photoId + '/setMain', {});
  }

  deletePhoto(userId: number, photoId: number) {
    return this.http.delete(this.baseUrl + 'users/' + userId + '/profile/' + photoId);
  }

  sendLike(id: number, userId: number) {
    return this.http.post(this.baseUrl + 'users/' + id + '/like/' + userId, {});
  }
}
