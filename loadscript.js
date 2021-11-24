import http from 'k6/http';
import { sleep } from 'k6';

export default function () {
  http.get('https://test.https://aa.seen.wtf/.io');
  sleep(1);
}