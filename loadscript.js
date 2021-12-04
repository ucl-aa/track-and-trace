import http from 'k6/http';
import { check, group, sleep } from 'k6';

 const url = 'https://aa.seen.wtf/';

 export const options = {
  stages: [
    { duration: '3m', target: 30 }, // simulate ramp-up of traffic from 1 to 30 users over 5 minutes.
    { duration: '3m', target: 30 }, // stay at 30 users for 10 minutes
    { duration: '3m', target: 50 }, // ramp-up to 50 users over 3 minutes (peak hour starts)
    { duration: '2m', target: 50 }, // stay at 50 users for short amount of time (peak hour)
    { duration: '3m', target: 30 }, // ramp-down to 30 users over 3 minutes (peak hour ends)
    { duration: '3m', target: 30 }, // continue at 30 for additional 10 minutes
    { duration: '3m', target: 0 }, // ramp-down to 0 users
  ],
  thresholds: {
    http_req_duration: ['p(99)<10000'], // 99% of requests must complete below 10s
  },
};


export default function () {
  http.get(url);
}

