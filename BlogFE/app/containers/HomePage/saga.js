import { call, put, select, takeLatest } from 'redux-saga/effects';
import { post, get } from '../../api/axiosApi';
import { GET_LIST_POST_VIEW_REQUEST } from './constants';
import { getListPostViewSuccess, getListPostViewError } from './actions';

export function* getListPostViewSaga({ categoryName, page }) {
  try {
    const response = yield call(get, `post/viewpost/${page}`);
    yield put(getListPostViewSuccess(response));
  } catch (error) {
    yield put(getListPostViewError(error));
  }
}

export default function* homePageSaga() {
  yield takeLatest(GET_LIST_POST_VIEW_REQUEST, getListPostViewSaga);
}
