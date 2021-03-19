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

const eventIds = ['80ef6e01-f1b7-4e27-ac83-9ab8910df475', '4a6c4285-50d1-4242-81ed-0e3efbca040d', '2c6ce1c5-146f-443c-976b-27dead85f748', '1456ba4a-9fe9-42d3-95b3-98ae5be4109d'];

export default function main() {
    let response;

    response = http.get(
        'http://localhost:5000/api/event/public?id=' + eventIds[Math.floor(Math.random() * eventIds.length)], {
            headers: {
                accept: "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
                "accept-encoding": "gzip, deflate, br",
                "accept-language": "en-US,en;q=0.9,pt;q=0.8",
                "cache-control": "max-age=0",
                connection: "keep-alive",
                dnt: "1",
                host: "localhost:5000",
                "sec-fetch-dest": "document",
                "sec-fetch-mode": "navigate",
                "sec-fetch-site": "same-origin",
                "sec-fetch-user": "?1",
                "upgrade-insecure-requests": "1",
            },
        }
    );
    check(response, {
        "status equals 200": response => response.status.toString() === "200",
    });

    // Automatically added sleep
    sleep(1);
}

export function handleSummary(data) {
    return {
        "StressEvents.html": htmlReport(data),
    };
}