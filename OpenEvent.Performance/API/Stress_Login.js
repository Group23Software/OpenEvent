import {
    sleep,
    check,
    group
} from "k6";
import http from "k6/http";
import {
    htmlReport
} from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

export const options = {
    stages: [{
            duration: "2m",
            target: 50
        },
        {
            duration: "2m",
            target: 50
        },
        {
            duration: "2m",
            target: 100
        },
        {
            duration: "2m",
            target: 100
        },
        {
            duration: "2m",
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
        check(response, {
            "status equals 200": response => response.status.toString() === "200",
            "$.Token is string": response =>
                jsonpath
                .query(response.json(), "$.Token")
                .some(value => typeof value === "string"),
        });
    });

    // Automatically added sleep
    sleep(1);
}

export function handleSummary(data) {
    return {
        "StressLogin.html": htmlReport(data),
    };
}