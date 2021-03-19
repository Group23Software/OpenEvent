import { sleep, check, group } from "k6";
import http from "k6/http";
import {
  htmlReport
} from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

export const options = {
  stages: [
    { duration: "1m", target: 10 },
    { duration: "3m", target: 10 },
    { duration: "1m", target: 0 },
  ],
  ext: {
    loadimpact: {
      distribution: {
        "amazon:gb:london": { loadZone: "amazon:gb:london", percent: 100 },
      },
    },
  },
};

export default function main() {
  let response;

  group("page_61 - http://localhost:5000/search", function () {
    response = http.get("http://localhost:5000/search", {
      headers: {
        Host: "localhost:5000",
        Connection: "keep-alive",
        DNT: "1",
        "Upgrade-Insecure-Requests": "1",
        Accept:
          "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
        "Sec-Fetch-Site": "none",
        "Sec-Fetch-Mode": "navigate",
        "Sec-Fetch-User": "?1",
        "Sec-Fetch-Dest": "document",
        "Accept-Encoding": "gzip, deflate, br",
        "Accept-Language": "en-US,en;q=0.9,pt;q=0.8",
      },
    });
    check(response, {
      "status equals 200": response => response.status.toString() === "200",
    });
    sleep(0.9);

    response = http.get("http://localhost:5000/api/event/categories", {
      headers: {
        Host: "localhost:5000",
        Connection: "keep-alive",
        Accept: "application/json, text/plain, */*",
        DNT: "1",
        Authorization: "Bearer",
        "Sec-Fetch-Site": "same-origin",
        "Sec-Fetch-Mode": "cors",
        "Sec-Fetch-Dest": "empty",
        "Accept-Encoding": "gzip, deflate, br",
        "Accept-Language": "en-US,en;q=0.9,pt;q=0.8",
      },
    });
    check(response, {
      "status equals 200": response => response.status.toString() === "200",
    });

    response = http.post(
      "http://localhost:5000/api/event/search?keyword=",
      "[]",
      {
        headers: {
          Host: "localhost:5000",
          Connection: "keep-alive",
          Accept: "application/json, text/plain, */*",
          DNT: "1",
          Authorization: "Bearer",
          "Content-Type": "application/json",
          Origin: "http://localhost:5000",
          "Sec-Fetch-Site": "same-origin",
          "Sec-Fetch-Mode": "cors",
          "Sec-Fetch-Dest": "empty",
          "Accept-Encoding": "gzip, deflate, br",
          "Accept-Language": "en-US,en;q=0.9,pt;q=0.8",
        },
      }
    );
    check(response, {
      "status equals 200": response => response.status.toString() === "200",
    });
  });
}

export function handleSummary(data) {
  return {
      "LoadSearchPageContent.html": htmlReport(data),
  };
}
