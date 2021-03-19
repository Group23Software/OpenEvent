// import http from 'k6/http';
// import {
//   sleep,
//   check
// } from 'k6';

// export let options = {
//   stages: [{
//       duration: '5m',
//       target: 2
//     }, // simulate ramp-up of traffic from 1 to 100 users over 5 minutes.
//     {
//       duration: '10m',
//       target: 2
//     }, // stay at 100 users for 10 minutes
//     {
//       duration: '5m',
//       target: 0
//     }, // ramp-down to 0 users
//   ]
// }

// const BASE_URL = 'http://localhost:5000/api';
// const EMAIL = 'harrison@thebarkers.me.uk';
// const PASSWORD = 'Password1@';

// const eventIds = ['80ef6e01-f1b7-4e27-ac83-9ab8910df475', '4a6c4285-50d1-4242-81ed-0e3efbca040d', '2c6ce1c5-146f-443c-976b-27dead85f748','1456ba4a-9fe9-42d3-95b3-98ae5be4109d'];

// export default () => {

//   // let params = {
//   //     headers: {
//   //         'Content-Type': 'application/json',
//   //     },
//   // };

//   // let res = http.post(BASE_URL + '/auth/login', `{"Email":"${EMAIL}","Password":"${PASSWORD}","Remember":false}`, params);

//   // check(res, {
//   //     'response code was 200': (res) => res.status == 200,
//   //   });

//   // sleep(1);

// //   fetch("http://localhost:5000/api/event/public?id=4a6c4285-50d1-4242-81ed-0e3efbca040d", {
// //   "headers": {
// //     "accept": "application/json, text/plain, */*",
// //     "accept-language": "en-US,en;q=0.9",
// //     "authorization": "Bearer",
// //     "sec-fetch-dest": "empty",
// //     "sec-fetch-mode": "cors",
// //     "sec-fetch-site": "same-site"
// //   },
// //   "referrer": "http://localhost:4200/",
// //   "referrerPolicy": "strict-origin-when-cross-origin",
// //   "body": null,
// //   "method": "GET",
// //   "mode": "cors"
// // });


// let url = BASE_URL + '/event/public?id=' + eventIds[Math.floor(Math.random() * eventIds.length)];
// console.log(url);
//   let res = http.get(url);


//   check(res, {
//     'response code was 200': (res) => res.status == 200,
//   });

//   sleep(1);
// }

export default function () {

// //   http.get('http://localhost:5000/api/user/GetUsersAnalytics?id=08d8d377-bea6-4d9f-883e-a300a744826d',{headers: {Authorization: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjA4ZDhkMzc3LWJlYTYtNGQ5Zi04ODNlLWEzMDBhNzQ0ODI2ZCIsIm5iZiI6MTYxMzU4OTA1MiwiZXhwIjoxNjE2MTgxMDUyLCJpYXQiOjE2MTM1ODkwNTJ9.zxqpZ5VVBYVuqojeucGHBLWZ6CdLZOXv7wfbxPRyw4Q'}});

// //   // http.get('http://test.k6.io');
// //   sleep(1);
}

// // curl 'http://localhost:5000/api/auth/login' \
// //   -H 'Connection: keep-alive' \
// //   -H 'Accept: application/json, text/plain, */*' \
// //   -H 'DNT: 1' \
// //   -H 'Authorization: Bearer' \
// //   -H 'User-Agent: Mozilla/5.0 (Macintosh; Intel Mac OS X 11_1_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.192 Safari/537.36' \
// //   -H 'Content-Type: application/json' \
// //   -H 'Origin: http://localhost:4200' \
// //   -H 'Sec-Fetch-Site: same-site' \
// //   -H 'Sec-Fetch-Mode: cors' \
// //   -H 'Sec-Fetch-Dest: empty' \
// //   -H 'Referer: http://localhost:4200/' \
// //   -H 'Accept-Language: en-US,en;q=0.9,pt;q=0.8' \
// //   --data-raw '{"Email":"harrison@thebarkers.me.uk","Password":"Password1@","Remember":false}' \
// //   --compressed