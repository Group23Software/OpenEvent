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

    group("Popular", function () {
        response = http.get("http://localhost:5000/api/popularity/categories", {
            headers: {
                accept: "application/json, text/plain, */*",
                "accept-encoding": "gzip, deflate, br",
                "accept-language": "en-US,en;q=0.9,pt;q=0.8",
                connection: "keep-alive",
                dnt: "1",
                host: "localhost:5000",
                "sec-fetch-dest": "empty",
                "sec-fetch-mode": "cors",
                "sec-fetch-site": "same-origin",
            },
        });
        check(response, {
            "status equals 200": response => response.status.toString() === "200",
        });

        // clone
        response = http.get("http://localhost:5000/api/popularity/events", {
            headers: {
                accept: "application/json, text/plain, */*",
                "accept-encoding": "gzip, deflate, br",
                "accept-language": "en-US,en;q=0.9,pt;q=0.8",
                connection: "keep-alive",
                dnt: "1",
                host: "localhost:5000",
                "sec-fetch-dest": "empty",
                "sec-fetch-mode": "cors",
                "sec-fetch-site": "same-origin",
            },
        });
        check(response, {
            "status equals 200": response => response.status.toString() === "200",
        });
    });

    // Automatically added sleep
    sleep(1);
}

export function handleSummary(data) {
    return {
        "StressPopular.html": htmlReport(data),
    };
}