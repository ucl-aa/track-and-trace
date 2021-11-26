import http from 'k6/http';
import { check, group, sleep } from 'k6';

 const url = 'https://aa.seen.wtf/';

 export const options = {
  stages: [
    { duration: '5m', target: 30 }, // simulate ramp-up of traffic from 1 to 60 users over 5 minutes.
    { duration: '5m', target: 30 }, // stay at 60 users for 10 minutes
    { duration: '3m', target: 50 }, // ramp-up to 100 users over 3 minutes (peak hour starts)
    { duration: '2m', target: 50 }, // stay at 100 users for short amount of time (peak hour)
    { duration: '3m', target: 30 }, // ramp-down to 60 users over 3 minutes (peak hour ends)
    { duration: '5m', target: 30 }, // continue at 60 for additional 10 minutes
    { duration: '5m', target: 0 }, // ramp-down to 0 users
  ],
  thresholds: {
    http_req_duration: ['p(99)<10000'], // 99% of requests must complete below 10s
  },
};


export default function () {
  http.get(url);
  sleep(3);
  http.get(url);
  sleep(3);
  http.get(url);
  sleep(3);
  http.get(url);
  sleep(3);
  http.get(url);
  sleep(3);
  http.get(url);
  sleep(3);
  http.get(url);
}
