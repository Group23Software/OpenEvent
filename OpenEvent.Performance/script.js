import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
  vus: 10,
  duration: '30s',
};

export default function () {

  http.get('http://localhost:5000/api/user/GetUsersAnalytics?id=08d8d377-bea6-4d9f-883e-a300a744826d',{headers: {Authorization: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjA4ZDhkMzc3LWJlYTYtNGQ5Zi04ODNlLWEzMDBhNzQ0ODI2ZCIsIm5iZiI6MTYxMzU4OTA1MiwiZXhwIjoxNjE2MTgxMDUyLCJpYXQiOjE2MTM1ODkwNTJ9.zxqpZ5VVBYVuqojeucGHBLWZ6CdLZOXv7wfbxPRyw4Q'}});

  // http.get('http://test.k6.io');
  sleep(1);
}
