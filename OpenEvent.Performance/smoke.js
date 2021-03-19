// import http from 'k6/http';
// import {
//     sleep,
//     check
// } from 'k6';

// export let options = {
//     vus: 10, // 1 user looping for 1 minute
//     duration: '1m',
//     thresholds: {
//         http_req_duration: ['p(99)<500'], // 99% of requests must complete below 1.5s
//     },
// };

// const BASE_URL = 'http://localhost:5000/api';
// const EMAIL = 'harrison@thebarkers.me.uk';
// const PASSWORD = 'Password1@';

export default () => {

//     let params = {
//         headers: {
//             'Content-Type': 'application/json',
//         },
    };

//     let res = http.post(BASE_URL + '/auth/login', `{"Email":"${EMAIL}","Password":"${PASSWORD}","Remember":false}`, params);

//     check(res, {
//         'response code was 200': (res) => res.status == 200,
//       });

//     sleep(1);
// }