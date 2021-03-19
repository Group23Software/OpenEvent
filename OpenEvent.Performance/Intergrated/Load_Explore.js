import {
    sleep,
    check,
    group
} from "k6";
import http from "k6/http";
import jsonpath from "https://jslib.k6.io/jsonpath/1.0.2/index.js";
import {
    htmlReport
} from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

export const options = {
    stages: [{
            duration: "1m",
            target: 10
        },
        {
            duration: "3m",
            target: 10
        },
        {
            duration: "1m",
            target: 0
        },
    ],
};

export default function main() {
    let response;

    group("Login", function () {
        response = http.post(
            "http://localhost:5000/api/auth/login",
            '{"Email":"harrison@thebarkers.me.uk","Password":"iD4gv0^TNgV^","Remember":false}', {
                headers: {
                    accept: "application/json, text/plain, */*",
                    dnt: "1",
                    authorization: "Bearer",
                    "content-type": "application/json",
                    origin: "http://localhost:5000",
                    "sec-fetch-site": "same-origin",
                    "sec-fetch-mode": "cors",
                    "sec-fetch-dest": "empty",
                    "accept-encoding": "gzip, deflate, br",
                    "accept-language": "en-US,en;q=0.9,pt;q=0.8",
                },
            }
        );

        let token = response.json().Token;
        let userId  = response.json().Id;

        check(response, {
            "status equals 200": response => response.status.toString() === "200",
            "$.Token is string": response =>
                jsonpath
                .query(response.json(), "$.Token")
                .some(value => typeof value === "string"),
        });

        response = http.get(
            `http://localhost:5000/api/event/explore?id=${userId}`, {
                headers: {
                    accept: "application/json, text/plain, */*",
                    dnt: "1",
                    authorization: `Bearer ${token}`,
                    "sec-fetch-site": "same-origin",
                    "sec-fetch-mode": "cors",
                    "sec-fetch-dest": "empty",
                    "accept-encoding": "gzip, deflate, br",
                    "accept-language": "en-US,en;q=0.9,pt;q=0.8",
                },
            }
        );

        check(response, {
            "status equals 200": response => response.status.toString() === "200",
        });
    });

    // Automatically added sleep
    sleep(1);
}

export function handleSummary(data) {
    return {
        "LoadExplore.html": htmlReport(data),
    };
}