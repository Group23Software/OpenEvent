import http from 'k6/http';
import { sleep, check } from 'k6';

export let options = {
  stages: [
    { duration: '5m', target: 100 }, // simulate ramp-up of traffic from 1 to 100 users over 5 minutes.
    { duration: '10m', target: 100 }, // stay at 100 users for 10 minutes
    { duration: '5m', target: 0 }, // ramp-down to 0 users
  ]
}

const BASE_URL = 'http://localhost:5000/api';
const EMAIL = 'harrison@thebarkers.me.uk';
const PASSWORD = 'Password1@';

export default () => {

    let params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    let res = http.post(BASE_URL + '/auth/login', `{"Email":"${EMAIL}","Password":"${PASSWORD}","Remember":false}`, params);

    check(res, {
        'response code was 200': (res) => res.status == 200,
      });

    sleep(1);
}

// export default function () {

//   http.get('http://localhost:5000/api/user/GetUsersAnalytics?id=08d8d377-bea6-4d9f-883e-a300a744826d',{headers: {Authorization: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjA4ZDhkMzc3LWJlYTYtNGQ5Zi04ODNlLWEzMDBhNzQ0ODI2ZCIsIm5iZiI6MTYxMzU4OTA1MiwiZXhwIjoxNjE2MTgxMDUyLCJpYXQiOjE2MTM1ODkwNTJ9.zxqpZ5VVBYVuqojeucGHBLWZ6CdLZOXv7wfbxPRyw4Q'}});

//   // http.get('http://test.k6.io');
//   sleep(1);
// }

// curl 'http://localhost:5000/api/auth/login' \
//   -H 'Connection: keep-alive' \
//   -H 'Accept: application/json, text/plain, */*' \
//   -H 'DNT: 1' \
//   -H 'Authorization: Bearer' \
//   -H 'User-Agent: Mozilla/5.0 (Macintosh; Intel Mac OS X 11_1_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.192 Safari/537.36' \
//   -H 'Content-Type: application/json' \
//   -H 'Origin: http://localhost:4200' \
//   -H 'Sec-Fetch-Site: same-site' \
//   -H 'Sec-Fetch-Mode: cors' \
//   -H 'Sec-Fetch-Dest: empty' \
//   -H 'Referer: http://localhost:4200/' \
//   -H 'Accept-Language: en-US,en;q=0.9,pt;q=0.8' \
//   --data-raw '{"Email":"harrison@thebarkers.me.uk","Password":"Password1@","Remember":false}' \
//   --compressed