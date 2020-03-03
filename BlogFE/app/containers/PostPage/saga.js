import { call, put, select, takeLatest } from 'redux-saga/effects';
import { post, get } from '../../api/axiosApi';
import { LOAD_POST_REQUEST } from './constants';
import { loadPostSuccess, loadPostError } from './actions';

export function* loadPostSaga({ id }) {
  try {
    const response = yield call(get, `post/${id}`);
    yield put(loadPostSuccess(response));
  } catch (error) {
    yield put(loadPostError(error));
  }
}

export default function* postPageSaga() {
  yield takeLatest(LOAD_POST_REQUEST, loadPostSaga);
}
