import { sleep, check, group } from "k6";
import http from "k6/http";
import {
  htmlReport
} from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";

export const options = {
  stages: [
    { duration: "12s", target: 1 },
    { duration: "36s", target: 1 },
    { duration: "12s", target: 0 },
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

  group("page_59 - http://localhost:5000/", function () {
    response = http.get("http://localhost:5000/", {
      headers: {
        Host: "localhost:5000",
        Connection: "keep-alive",
        "Cache-Control": "max-age=0",
        DNT: "1",
        "Upgrade-Insecure-Requests": "1",
        Accept:
          "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9",
        "Sec-Fetch-Site": "same-origin",
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
    sleep(0.8);

    response = http.post(
      "http://localhost:5000/popularityHub/negotiate?negotiateVersion=1",
      null,
      {
        headers: {
          Host: "localhost:5000",
          Connection: "keep-alive",
          "Cache-Control": "max-age=0",
          DNT: "1",
          "X-Requested-With": "XMLHttpRequest",
          "X-SignalR-User-Agent":
            "Microsoft SignalR/5.0 (5.0.4; Unknown OS; Browser; Unknown Runtime Version)",
          "Content-Type": "text/plain; charset=UTF-8",
          Accept: "*/*",
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

    response = http.get("http://localhost:5000/api/popularity/events", {
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

    response = http.get("http://localhost:5000/api/popularity/categories", {
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

    response = http.get(
      "ws://localhost:5000/popularityHub?id=k6EmselooSTrlBDf1IiacQ",
      {
        headers: {
          Origin: "http://localhost:5000",
          "Accept-Encoding": "gzip, deflate, br",
          Host: "localhost:5000",
          "Accept-Language": "en-US,en;q=0.9,pt;q=0.8",
          "Sec-WebSocket-Key": "p5yMzAvn/AMOG4BemuRBBw==",
          Upgrade: "websocket",
          "Sec-WebSocket-Extensions":
            "permessage-deflate; client_max_window_bits",
          "Cache-Control": "no-cache",
          Connection: "Upgrade",
          "Sec-WebSocket-Version": "13",
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
      "LoadLandingPageContent.html": htmlReport(data),
  };
}